using System.Collections.Generic;

namespace Controllers.Subsystems.Story
{
    public class StoryRecorder
    {
        public StoryRecorder()
        {
            m_record = new Dictionary<string, List<StoryAction>>();
        }
        
        public void PushRecord(string id, StoryAction action)
        {
            if (m_preID != id && m_record.ContainsKey(id))
            {
                id = DealTheSameID(id);
            }
            
            if (!m_record.ContainsKey(id))
            {
                m_record[id]= new List<StoryAction>();
            }

            switch (action.Type)
            {
                case StoryActionType.Bold:
                case StoryActionType.Color:
                case StoryActionType.Font:
                case StoryActionType.Waiting:
                case StoryActionType.FontSize:
                case StoryActionType.Picture:
                case StoryActionType.PictureMove:
                case StoryActionType.ChangeEffectMusic:
                case StoryActionType.ChangeBGM:
                    break;
                case StoryActionType.Content:
                    PushContent(id, action);
                    break;
                case StoryActionType.Name:
                case StoryActionType.Jump:
                    m_record[id].Add(action);
                    break;
            }
        }

        public List<StoryAction> GetRecords()
        {
            List<StoryAction> records = new List<StoryAction>();
            foreach (var record in m_record)
            {
                records.AddRange(record.Value);
            }

            return records;
        }

        public void ClearRecords()
        {
            foreach (var record in m_record)
            {
                record.Value.Clear();
            }
            m_record.Clear();
        }

        private void PushContent(string id,StoryAction content)
        {
            int lastActionIndex = m_record[id].Count -1;
            if (lastActionIndex <= 0)
            {
                return;
            }

            StoryAction action = m_record[id][lastActionIndex];
            if (action.Type == StoryActionType.Content)
            {
                action.Content = content.Content;
            }
        }

        private string DealTheSameID(string id)
        {
            int i = 1;
            string newID = id;
            while (m_record.ContainsKey(newID))
            {
                newID = id + "_Record_" + i;
                i++;
            }

            return newID;
        }

        private string m_preID;
        private Dictionary<string,List<StoryAction>> m_record;
    }
}