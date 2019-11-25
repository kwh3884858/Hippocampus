#ifndef StoryJson_hpp
#define StoryJson_hpp

#include <stdio.h>

#include <vector>
#include "nlohmann/json.hpp"
#include "HeavenGateEditorConstant.h"


using json = nlohmann::json;
using std::string;
using std::vector;
namespace HeavenGateEditor {

    struct StoryWord {
        public:

        char m_name[MAX_NAME];
        char m_content[MAX_CONTENT];
    };

    class StoryJson {
    private:
        vector<StoryWord*> m_words;

    public:
        int AddWord(StoryWord* const word);
        int AddWord(string name, string content);
        int AddWord(const char* name, const char* content);

        void SetWord(const StoryWord* const word);

        StoryWord* const GetWord(int index);
        const StoryWord* const GetWord(int index) const;

        void RemoveWord();
        StoryWord FindWord(int index)const;
        int Size() const;
        void Clear();
        bool Empty()const;
    };

    void to_json(json& j, const StoryWord& p);
    void to_json(json& j, const StoryJson& p);

    void from_json(const json& j, StoryWord& p);
    void from_json(const json& j, StoryJson& p);

  

    //void to_json(json& j, const StoryWord* p) {
    //    j = json{ {"name", p->m_name}, {"content", p->m_content} };
    //}

    //void from_json(const json& j, StoryWord* p) {
    //    j.at("name").get_to(p.m_name);
    //    j.at("content").get_to(p.m_content);
    //}
    //void to_json(json& j, const StoryJson& p){
    //    for (int i = 0 ; i < p.m_words.size(); i++) {
    //
    //    }
    //}
    //
    // void from_json(const json& j, StoryJson& p) {
    //
    //     }
}

#endif /* StoryJson_hpp */
