using Controllers.Subsystems.Story;
using Evidence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
			chapter,
			scene,
			character,
			pause,
			exhibit
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
							break;
						case TableType.color:
							m_container.PushColor (m_currentColor);
							break;
						case TableType.tip:
							break;
						case TableType.paintMove:
							break;
						case TableType.pause:
							break;
						case TableType.exhibit:
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
						if (token.m_content == TableType.paintMove.ToString ()) {
							editorState.Push (TableType.paintMove);
						}
						if (token.m_content == TableType.pause.ToString ()) {
							editorState.Push (TableType.pause);
						}
						if (token.m_content == TableType.exhibit.ToString ()) {
							editorState.Push (TableType.exhibit);
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

						break;

					case TableType.color:

						m_container.PushColor (token.m_content);
						m_currentColor = token.m_content;
						break;

					case TableType.tip:
						break;
					case TableType.paintMove:
						break;
					case TableType.pause:
						break;
					case TableType.exhibit:
						EvidenceDataManager.Instance.AddEvidence (token.m_content);
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

		StoryCompiler m_compiler;
		StoryActionContainer m_container;
	}
}