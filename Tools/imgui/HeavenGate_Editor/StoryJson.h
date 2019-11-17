//
//  StoryJson.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 14/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef StoryJson_hpp
#define StoryJson_hpp

#include <stdio.h>
#include <string>
#include <vector>
#include "nlohmann/json.hpp"



using json = nlohmann::json;
using std::string;
using std::vector;
namespace HeavenGateEditor {

    class StoryWord {
        public:

        string m_name;
        string m_content;
    };

    class StoryJson {
    private:
        vector<StoryWord *> m_words;

    public:
        void AddWord();
        void RemoveWord();
        StoryWord FindWord(int index)const;
        void Size() const;
        void Clear();
        bool Empty()const;
    };

    void to_json(json& j, const StoryWord& p){
        j = json{{"name", p.m_name}, {"content", p.m_content}};
    
    }
    
    void from_json(const json& j, StoryWord& p) {
        j.at("name").get_to(p.m_name);
             j.at("content").get_to(p.m_content);
    }

    void to_json(json& j, const StoryWord* p) {
        j = json{ {"name", p->m_name}, {"content", p->m_content} };
    }

    void from_json(const json& j, StoryWord* p) {
        j.at("name").get_to(p.m_name);
        j.at("content").get_to(p.m_content);
    }
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
