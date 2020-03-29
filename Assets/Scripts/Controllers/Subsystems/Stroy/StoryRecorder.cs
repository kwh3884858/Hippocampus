using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Subsystems.Story
{
    public enum RecordType
    {
        TalkContent,
        Jump,
    }

    public class RecordData
    {
        public RecordType Type;
        public string str1;
        public string str2;

        public RecordData()
        {
            str1 = String.Empty;
            str2 = String.Empty;
        }
    }
    public class StoryRecorder
    {
        public StoryRecorder()
        {
            m_record = new Dictionary<string, List<RecordData>>();
        }
        
        public void PushRecord(string id, StoryAction action)
        {
            if (m_preID != id && m_record.ContainsKey(id))
            {
                id = DealTheSameID(id);
            }
            
            if (!m_record.ContainsKey(id))
            {
                m_record[id]= new List<RecordData>();
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
                    PushName(id,action);
                    break;
                case StoryActionType.Jump:
                    PushJump(id,action as StoryJumpAction);
                    break;
            }
        }

        public List<RecordData> GetRecords()
        {
            List<RecordData> records = new List<RecordData>();
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

            RecordData action = m_record[id][lastActionIndex];
            if (action.Type == RecordType.TalkContent)
            {
                action.str2 += content.Content;
            }
            else
            {
                Debug.LogWarning($"StoryRecorder 记录对话数据错误，请检查 ID:{id},Type:{content.Type}");
            }
        }

        private void PushJump(string id, StoryJumpAction action)
        {

            if (m_record.ContainsKey(id) || action == null || action.Options.Count == 0)
            {
                return;
            }
            m_record[id].Add(new RecordData(){Type = RecordType.Jump,str1 = action.Options[0].Content});
        }

        private void PushName(string id, StoryAction action)
        {
            if (m_record.ContainsKey(id) || action == null)
            {
                return;
            }
            m_record[id].Add(new RecordData(){Type = RecordType.TalkContent,str1 = action.Content});
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
        private Dictionary<string,List<RecordData>> m_record;
    }
}