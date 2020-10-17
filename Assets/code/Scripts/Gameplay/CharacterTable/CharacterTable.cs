using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTable
{
    [System.Serializable]
    public class CharacterValue
    {
        public string characterName;
        public string tachieId;
    }

    public class CharacterLayout
    {
        public string[] header;
        public string type;
        public CharacterValue[] value;
    }

    public CharacterTable(string rawTableData)
    {
        CharacterLayout layout = JsonUtility.FromJson<CharacterLayout>(rawTableData);
        m_characterValue = layout.value;
        if (m_characterValue == null)
        {
            Debug.LogError("Character table data is wrong.");
        }
    }

    public string GetTachieID(string characterName)
    {
        for (int i = 0; i < m_characterValue.Length; i++)
        {
            if (m_characterValue[i].characterName == characterName)
            {
                return m_characterValue[i].tachieId;
            }
        }
        Debug.LogError("Can`t find the character name: " + characterName);
        return null;
    }
    public string GetCharacterName(string tachieId)
    {
        for (int i = 0; i < m_characterValue.Length; i++)
        {
            if (m_characterValue[i].tachieId == tachieId)
            {
                return m_characterValue[i].characterName;
            }
        }
        Debug.LogWarning("Can`t find the tachie ID: " + tachieId);

        return null;
    }

    CharacterValue[] m_characterValue;
}
