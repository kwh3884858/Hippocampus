using Controllers.Subsystems.Story;
using Evidence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;
using System;

namespace StarPlatinum.StoryCompile
{
	using Token = StoryCompiler.Token;

	public class StoryVirtualMachine : Singleton<StoryVirtualMachine>
	{


		enum TableType
		{
			none,
			fontSize,
			color,
			tip,
			paintMove,
			//chapter,
			//scene,
			character,
			pause,
			exhibit,
			effect,
			bgm,
			tachie
		};


		public StoryVirtualMachine ()
		{
			m_compiler = new StoryCompiler ();
			m_currentColor = "FFFFFFFF";
		}

		public void Run (string content)
		{
			if (m_container == null) return;

			List<Token> tokens = m_compiler.Compile (content);

			Parser (ref tokens);
		}

		public void SetStoryActionContainer (StoryActionContainer container)
		{
			m_container = container;
		}
		void Parser (ref List<Token> m_tokens)
		{
			TableType currentState;
			Stack<TableType> editorState = new Stack<TableType> ();
			bool isReadCloseLabel;

			isReadCloseLabel = false;
			currentState = TableType.none;

			for (int i = 0; i < m_tokens.Count; i++) {
				Token token = m_tokens [i];
				if (token.m_tokeType == StoryCompiler.TokenType.TokenInstructor) {
					if (isReadCloseLabel) {
						// Is closing state
						if (editorState.Count == 0) {
							Debug.Log ("Close Label is lack\n");
							continue;
						}

						currentState = editorState.Pop ();

						isReadCloseLabel = false;

						switch (currentState) {
						case TableType.none:
							break;
						case TableType.fontSize:
							m_container.PushFontSize (m_currentFontSize);
							break;
						case TableType.color:
							m_container.PushColor (m_currentColor);
							break;
						case TableType.tip:
							break;
						case TableType.paintMove:
							break;
						case TableType.pause:
							m_container.PushTypeWriterInterval (m_currentTypeWriterInterval);
							break;
						case TableType.exhibit:
							break;
						case TableType.bgm:
							break;
						case TableType.effect:
							break;
						case TableType.tachie:
							break;
						default:
							break;
						}
					} else {
						// Is start state
						if (token.m_content == TableType.fontSize.ToString ()) {
							editorState.Push (TableType.fontSize);
						}
						if (token.m_content == TableType.color.ToString ()) {
							editorState.Push (TableType.color);
						}
						if (token.m_content == TableType.tip.ToString ()) {
							editorState.Push (TableType.tip);
						}
						if (token.m_content == TableType.paintMove.ToString ()) {
							editorState.Push (TableType.paintMove);
						}
						if (token.m_content == TableType.pause.ToString ()) {
							editorState.Push (TableType.pause);
						}
						if (token.m_content == TableType.exhibit.ToString ()) {
							editorState.Push (TableType.exhibit);
						}
						if (token.m_content == TableType.bgm.ToString ()) {
							editorState.Push (TableType.bgm);
						}
						if (token.m_content == TableType.effect.ToString ()) {
							editorState.Push (TableType.effect);
						}
						if (token.m_content == TableType.tachie.ToString ()) {
							editorState.Push (TableType.tachie);
						}
					}
				} else if (token.m_tokeType == StoryCompiler.TokenType.TokenIdentity) {
					if (editorState.Count == 0) {
						Debug.Log ("No Identity Exist. \n");
						continue;
					}
					switch (editorState.Peek ()) {
					case TableType.none:
						break;
					case TableType.fontSize:
						m_currentFontSize = token.m_content;
						m_container.PushFontSize (m_currentFontSize);
						break;

					case TableType.color:

						m_container.PushColor (token.m_content);
						m_currentColor = token.m_content;
						break;

					case TableType.tip:
						//TODO: Add Tip
						//m_tipManager.addTip(token.m_content);
						Tips.TipsManager.Instance.UnlockTip (token.m_content, Tips.TipsManager.ConvertDateTimeToLong (System.DateTime.Now));// 添加tip 数据
						break;
					case TableType.paintMove:
						break;
					case TableType.pause:
						m_currentTypeWriterInterval = float.Parse (token.m_content);
						m_container.PushTypeWriterInterval (m_currentTypeWriterInterval);
						break;
					case TableType.exhibit:
						EvidenceDataManager.Instance.AddEvidence (token.m_content);
						break;
					case TableType.bgm:
						m_container.PushChangeBGM (token.m_content);
						break;
					case TableType.effect:
						m_container.PushPlayerEffectMusic (token.m_content);
						break;
					case TableType.tachie:
						string [] words = token.m_content.Split ('+');
						string [] pos = words [1].Split (',');
						m_container.PushPicture (words [0], Int32.Parse (pos [0]) + 100, Int32.Parse (pos [1]) + 100);
						break;
					default:
						break;
					}

				} else if (token.m_tokeType == StoryCompiler.TokenType.TokenOpSlash) {
					//Start close
					isReadCloseLabel = true;
				} else if (token.m_tokeType == StoryCompiler.TokenType.TokenContent) {
					m_container.PushContent (token.m_content);
				}

			}
		}

		string m_currentColor;
		string m_currentFontSize;
		float m_currentTypeWriterInterval;

		StoryCompiler m_compiler;
		StoryActionContainer m_container;
	}
}