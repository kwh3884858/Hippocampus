//
//  StoryJsonLabelNode.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#include "StoryJsonLabelNode.h"
#include "JsonUtility.h"

namespace HeavenGateEditor {

    enum class LabelLayout {
        NodeTypeName = 0,
        Label = 1
    };
    extern char labelNodeString[][MAX_ENUM_LENGTH] = {
        "typename",
        "label"
    };


    StoryLabel::StoryLabel()
    {
        m_nodeType = NodeType::Label;
    }

    StoryLabel::StoryLabel(const StoryLabel& storyLabel)
    {
        m_nodeType = storyLabel.m_nodeType;
        strcpy(m_labelId, storyLabel.m_labelId);
    }


    void to_json(json& j, const StoryLabel& p)
    {
        j = json{
            {labelNodeString[(int)LabelLayout::NodeTypeName],    nodeTypeString[(int)p.m_nodeType] },
            {labelNodeString[(int)LabelLayout::Label],       p.m_labelId}
        };
    }


    void from_json(const json& j, StoryLabel& p) {
        p.m_nodeType = NodeType::Label;
        //strcpy(p.m_labelId, j.at(labelNodeString[(int)LabelLayout::Label]).get_ptr<const json::string_t *>()->c_str());
        GetCharPointerException(p.m_labelId, j, labelNodeString[(int)LabelLayout::Label]);
    }

}
