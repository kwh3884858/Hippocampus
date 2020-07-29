using System;
using System.Collections.Generic;
using GamePlay.Stage;
using StarPlatinum.EventManager;
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
        public Vector2 Distance { get; set; }
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
            posX -= 100;
            posY -= 100;
            m_actions.Enqueue(new StoryShowPictureAction(){Type = StoryActionType.Picture,Content = picID,Pos = new Vector2(posX,posY)});
        }
        
        public void PushPicMove(string picID,Vector2 distance,float duration)
        {
            m_actions.Enqueue(new StoryPictureMoveAction(){Type = StoryActionType.PictureMove,Content = picID,Distance = distance,Duration = duration});
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

        public void PushShowEvidence(string evidenceID)
        {
            m_actions.Enqueue(new StoryAction(){Type =  StoryActionType.ShowEvidence , Content = evidenceID});
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

            while (m_actions.Count>0)
            {
                var storyAction = GetNextAction();

                switch (storyAction.Type)
                {
                    case StoryActionType.Jump:
                    case StoryActionType.LoadGameScene:
                    case StoryActionType.LoadMission:
                    case StoryActionType.ShowEvidence:
                        if (isTalk)
                        {
                            actions.Enqueue(new StoryAction() {Type = StoryActionType.WaitClick});
                        }
                        actions.Enqueue(storyAction);
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
                    default:
                        actions.Enqueue(storyAction);
                        break;
                }
            }

            while (actions.Count>0)
            {
                m_actions.Enqueue(actions.Dequeue());
            }
        }
        
        private Queue<StoryAction> m_actions;
        
    }
}