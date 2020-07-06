//
//  StoryJsonWordNode.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#include "StoryJsonWordNode.h"
#include "JsonUtility.h"

namespace HeavenGateEditor {

    enum class WordLayout {
        NodeTypeName = 0,
        Name,
        Content
    };

    char wordNodeString[][MAX_ENUM_LENGTH] = {
        "typename",
        "name",
        "content"
    };



    StoryWord::StoryWord()
    {
        m_nodeType = NodeType::Word;
    }

    StoryWord::StoryWord(const StoryWord& storyWard)
    {
        m_nodeType = storyWard.m_nodeType;
        strcpy(m_name, storyWard.m_name);
        strcpy(m_content, storyWard.m_content);

    }

    void to_json(json & j, const StoryWord & p)
    {
        j = json{
            {wordNodeString[(int)WordLayout::NodeTypeName],  nodeTypeString[(int)p.m_nodeType] },
            {wordNodeString[(int)WordLayout::Name],          p.m_name},
            {wordNodeString[(int)WordLayout::Content],       p.m_content }
        };

    }

    void from_json(const json & j, StoryWord & p)
    {
        p.m_nodeType = NodeType::Word;
        //strcpy(p.m_name, j.at(wordNodeString[(int)WordLayout::Name]).get_ptr<const json::string_t *>()->c_str());
        //strcpy(p.m_content, j.at(wordNodeString[(int)WordLayout::Content]).get_ptr<const json::string_t *>()->c_str());
        GetContentException(p.m_name, j, wordNodeString[(int)WordLayout::Name]);
        GetContentException(p.m_content, j, wordNodeString[(int)WordLayout::Content]);

    }
}
