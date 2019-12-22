using System.Collections.Generic;
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
        public string NextID { get; set; }
    }

    public class StoryPictureMoveAction : StoryAction
    {
        public Vector2 Distance { get; set; }
        public float Duration { get; set; }
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

        public void PushJump(string content, string nextID)
        {
            m_actions.Enqueue(new StoryJumpAction(){Type = StoryActionType.Jump,Content = content,NextID = nextID});
        }

        public void PushWaiting(float waitingTime)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.Waiting,Content = waitingTime.ToString()});
        }

        public void PushPicture(string picID)
        {
            m_actions.Enqueue(new StoryAction(){Type = StoryActionType.Picture,Content = picID});
        }
        
        public void PushPicMove(string picID,Vector2 distance,float duration)
        {
            m_actions.Enqueue(new StoryPictureMoveAction(){Type = StoryActionType.PictureMove,Content = picID,Distance = distance,Duration = duration});
        }

        public void PushBold()
        {
            m_actions.Enqueue(new StoryPictureMoveAction(){Type = StoryActionType.Bold});
        }

        public StoryAction GetNextAction()
        {
            if (m_actions.Count > 0)
            {
                return m_actions.Dequeue();

            }

            return null;
        }
        
        private Queue<StoryAction> m_actions;
        
    }
}