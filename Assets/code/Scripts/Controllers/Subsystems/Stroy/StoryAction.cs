﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Stage;
using StarPlatinum.EventManager;
using UI.Panels;
using UI.Panels.StaticBoard;
using UnityEngine;

namespace Controllers.Subsystems.Story
{
    public class StoryAction
    {
        public StoryActionType Type { get; set; }
        public string Content{ get; set; }
    }

    public class StoryContentAction : StoryAction
    {
        public bool IsNewBegin { get; set; }
    }
    public class StoryJumpAction : StoryAction
    {
        public List<Option> Options { get; set; }
    }

    public class StoryPictureMoveAction : StoryAction
    {
        public string PicID { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public Ease Ease { get; set; }
        public float Duration { get; set; }
    }

    public class StoryShowPictureAction : StoryAction
    {
        public Vector2 Pos { get; set; }
    }

    public class StoryLoadMissionAction : StoryAction
    {
        public MissionEnum Mission { get; set; }
    }

    public class StoryEventAction : StoryAction
    {
        public RaiseEvent Event { get; set; }
    }

    public class StoryEvidenceAction : StoryAction
    {
        public string evidenceID;
        public string prefix;
    }

    public class StoryLoadSkyboxAction : StoryAction
    {
        public SkyboxEnum m_skyEnum;
    }

    public class StoryActionContainer
    {
        public StoryActionContainer()
        {
            m_actions = new Queue<StoryAction>();
        }

        public void PushColor(string color)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.Color,Content = color});
        }

        public void PushName(string name)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.Name,Content = name});
        }

        public void PushContent(string content)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.Content,Content = content});
        }

        public void PushFontSize(string fontSize)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.FontSize,Content = fontSize});
        }

        public void PushFontName(string fontName)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.Font,Content = fontName});
        }

        public void PushJump(List<Option> options)
        {
            m_actions.Enqueue(new StoryJumpAction(){Type = StoryActionType.Jump,Options = options});
        }

        public void PushWaiting(float waitingTime)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.Waiting,Content = waitingTime.ToString()});
        }

        public void PushPicture(string picID,int posX,int posY=150)
        {
            posX = ProcessPicPos(posX);
            posY = ProcessPicPos(posY);
            m_actions.Enqueue(new StoryShowPictureAction(){Type = StoryActionType.Picture,Content = picID,Pos = new Vector2(posX,posY)});
        }
        
        public void PushPicMove(string picID,int startX,int startY,int endX,int endY,Ease ease,float duration)
        {
            startX = ProcessPicPos(startX);
            startY = ProcessPicPos(startY);
            endX = ProcessPicPos(endX);
            endY = ProcessPicPos(endY);

            m_actions.Enqueue(new StoryPictureMoveAction(){Type = StoryActionType.PictureMove,PicID = picID,StartX = startX,StartY = startY,EndX = endX,EndY = endY,Ease = ease,Duration = duration});
        }

        public void PushBold()
        {
            m_actions.Enqueue(new StoryPictureMoveAction(){Type = StoryActionType.Bold});
        }

        public void PushChangeBGM(string musicName)
        {
            m_actions.Enqueue(new StoryAction(){Type =  StoryActionType.ChangeBGM,Content = musicName});
        }

        public void PushPlayerEffectMusic(string effectName)
        {
            m_actions.Enqueue(new StoryAction(){Type =  StoryActionType.ChangeEffectMusic,Content = effectName});
        }

        public void PushShowEvidence(string evidenceID,string prefix)
        {
            m_actions.Enqueue(new StoryEvidenceAction(){Type =  StoryActionType.ShowEvidence , evidenceID = evidenceID, prefix = prefix});
        }

        public void PushTypeWriterInterval(float intervalTime)
        {
            m_actions.Enqueue(new StoryAction(){Type =  StoryActionType.TypewriterInterval , Content = intervalTime.ToString()});

        }

        public void PushLoadGameScene(string sceneID)
        {
            m_actions.Enqueue(new StoryAction(){Type =  StoryActionType.LoadGameScene , Content = sceneID});
        }

        public void LoadMission(MissionEnum mission)
        {
            m_actions.Enqueue(new StoryLoadMissionAction(){Type =  StoryActionType.LoadMission ,  Mission= mission});
        }

        public void LoadCGScene(string cgSceneName)
        {
            m_actions.Enqueue(new StoryAction() { Type = StoryActionType.LoadCgScene, Content = cgSceneName });
        }

        public void CloseCGScene(string cgSceneName)
        {
            m_actions.Enqueue(new StoryAction() { Type = StoryActionType.CloseCgScene, Content = cgSceneName });
        }
        public void TriggerEvent(RaiseEvent raiseEvent)
        {
            m_actions.Enqueue(new StoryEventAction(){Type =  StoryActionType.TriggerEvent , Event = raiseEvent});
        }

        public void PlayAnimation(string animtionID)
        {
            m_actions.Enqueue(new StoryAction(){Type =  StoryActionType.PlayAnimation , Content = animtionID});
        }

        public void ChangeBackground(string backgroundKey)
        {
            m_actions.Enqueue(new StoryAction(){Type =  StoryActionType.ChangeBackground , Content = backgroundKey});
        }

        public void LoadSkybox(SkyboxEnum skyboxEnum)
        {
            m_actions.Enqueue(new StoryLoadSkyboxAction() { Type = StoryActionType.LoadSkybox, m_skyEnum = skyboxEnum });
        }

        public void RemoveAllExhibit()
        {
            m_actions.Enqueue(new StoryAction() { Type = StoryActionType.RemoveAllExhibit });
        }

        public void PushWrap()
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.Wrap});
        }

        public void PushVolume(int volumePercentage)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.ChangeSoundVolume, Content = volumePercentage.ToString()});
        }

        public void PushEnterControversy(string ID)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.EnterControversy, Content = ID});
        }

        public void PushCutIn(string ImgKey)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.CutIn, Content = ImgKey});
        }

        public void PlayInteractionAnimation(string itemName)
        {
            m_actions.Enqueue(new StoryAction() { Type = StoryActionType.PlayInteractionAnimation, Content = itemName });
        }

        public void PushChangePanelType(int panelType)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.ChangeTalkPanelType, Content = panelType.ToString()});
        }

        public void PushFrontImg(string imgKey)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.ChangeFrontImg, Content = imgKey});
        }

        public void PushTimeLine(string key)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.TimeLine,Content = key});
        }

        private int ProcessPicPos(int pos)
        {
            return pos - 100;
        }

        public StoryAction GetNextAction()
        {
            if (m_actions.Count > 0)
            {
                return m_actions.Dequeue();

            }

            return null;
        }
        
        public void ProcessActionContainer()
        {
            bool isTalk = false;
            Queue<StoryAction> actions = new Queue<StoryAction>();
            int lastContentIndex=-1;
            while (m_actions.Count>0)
            {
                var storyAction = GetNextAction();

                switch (storyAction.Type)
                {
                    case StoryActionType.Jump:
                    case StoryActionType.LoadGameScene:
                    case StoryActionType.LoadMission:
                    case StoryActionType.LoadCgScene:
                    case StoryActionType.CloseCgScene:
                    case StoryActionType.ShowEvidence:
                    case StoryActionType.TimeLine:
                        if (isTalk)
                        {
                            actions.Enqueue(new StoryAction() {Type = StoryActionType.WaitClick});
                        }
                        actions.Enqueue(storyAction);
                        isTalk = false;
                        break;
                    case StoryActionType.ChangeFrontImg:
                        if (isTalk)
                        {
                            actions.Enqueue(new StoryAction() {Type = StoryActionType.WaitClick});
                        }
                        actions.Enqueue(storyAction);
                        actions.Enqueue(new StoryAction() {Type = StoryActionType.WaitClick});
                        actions.Enqueue(new StoryAction(){Type = StoryActionType.ChangeFrontImg});
                        isTalk = false;
                        break;
                    case StoryActionType.Name:
                        if (isTalk)
                        {
                            actions.Enqueue(new StoryAction() {Type = StoryActionType.WaitClick});
                        }
                        actions.Enqueue(storyAction);
                        isTalk = true;
                        break;
                    case StoryActionType.Content:
                        lastContentIndex = actions.Count;
                        actions.Enqueue(storyAction);
                        break;
                    default:
                        actions.Enqueue(storyAction);
                        break;
                }

            }

            int i = 0;
            while (actions.Count>0)
            {
                m_actions.Enqueue(actions.Dequeue());
                if (lastContentIndex == i)
                {
                    m_actions.Enqueue(new StoryAction(){Type = StoryActionType.WaitClick});
                }
                i++;
            }
        }
        
        private Queue<StoryAction> m_actions;
        
    }
}