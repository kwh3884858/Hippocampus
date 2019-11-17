#include "StoryJson.h"


void HeavenGateEditor::StoryJson::AddWord(StoryWord * const word)
{
    m_words.push_back(word);
}

void HeavenGateEditor::StoryJson::AddWord(string name, string content)
{
    StoryWord* word = new StoryWord();
    word->m_name = name;
    word->m_content = content;

    AddWord(word);
}

const HeavenGateEditor::StoryWord * const HeavenGateEditor::StoryJson::GetWord(int index) const
{
    return m_words[index];
}

HeavenGateEditor::StoryWord * const HeavenGateEditor::StoryJson::GetWord(int index)
{
    return const_cast<StoryWord*>(
        static_cast<const StoryJson&>(*this).GetWord(index));
}

int HeavenGateEditor::StoryJson::Size() const
{
    return m_words.size();
}

void HeavenGateEditor::to_json(json & j, const StoryWord & p)
{
    j = json{ {"name", p.m_name},{ "content", p.m_content } };
   
}

void HeavenGateEditor::to_json(json & j, const StoryJson & story)
{
    for (int i = 0; i < story.Size(); i++)
    {
        const StoryWord* const pWord = story.GetWord(i);
        string key = std::to_string(i);
        
        j[key] = *pWord;
    }
}

void HeavenGateEditor::from_json(const json & j, StoryWord & p)
{
    j.at("name").get_to(p.m_name);
    j.at("content").get_to(p.m_content);
}

void HeavenGateEditor::from_json(const json & j, StoryJson & p)
{
    for (int i = 0 ; i < j.size(); i++)
    {
        StoryWord* word = new StoryWord();
        *word = j[i];
        p.AddWord(word);

    }
    // iterate the array
   /* for (json::iterator it = j.begin(); it != j.end(); ++it) {
        StoryWord* word = new StoryWord();
        *word = *it;
        p.AddWord(word);
    }*/
}
