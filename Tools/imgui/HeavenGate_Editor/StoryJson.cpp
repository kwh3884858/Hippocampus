#include "StoryJson.h"
#include <string>
using std::string;

namespace HeavenGateEditor {

int HeavenGateEditor::StoryJson::AddWord(StoryWord * const word)
{
    m_words.push_back(word);
    return static_cast<int>( m_words.size());
}

int HeavenGateEditor::StoryJson::AddWord(string name, string content)
{
    return  AddWord(name.c_str(), content.c_str());
}

int  HeavenGateEditor::StoryJson::AddWord(const char* name, const char* content){
    StoryWord* word = new StoryWord();
    strcpy(word->m_name, name);
    strcpy(word->m_content, content);

    return  AddWord(word);
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
    return  static_cast<int>( m_words.size() );
}

void to_json(json & j, const StoryWord & p)
{
    j = json{ {"name", p.m_name},{ "content", p.m_content } };
   
}

void to_json(json & j, const StoryJson & story)
{
    for (int i = 0; i < story.Size(); i++)
    {
        const StoryWord* const pWord = story.GetWord(i);
        string key = std::to_string(i);
        
        j.push_back(*pWord);
    }
}

/* String version, Archieved */

//void from_json(const json & j, StoryWord & p)
//{
//    j.at("name").get_to(p.m_name);
//    j.at("content").get_to(p.m_content);
//}

void from_json(const json & j, StoryWord & p)
{
    //TODO: Directly transform string as character array
    strcpy( p.m_name, j.at("name").get_ptr<const json::string_t *>()->c_str() );
    strcpy( p.m_content, j.at("content").get_ptr<const json::string_t *>()->c_str() );

}

void from_json(const json & j, StoryJson & p)
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
}
