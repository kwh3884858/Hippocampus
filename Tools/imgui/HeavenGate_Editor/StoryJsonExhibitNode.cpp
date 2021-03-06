//
//  StoryJsonExhibitNode.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#include "StoryJsonExhibitNode.h"
#include "JsonUtility.h"

namespace HeavenGateEditor {

    enum class ExhibitLayout {
        NodeTypeName = 0,
        Name,
        ExhibitPrefix
    };

    extern char exhibitString[][MAX_ENUM_LENGTH] = {
        "typename",
        "exhibitName",
        "exhibitPrefix"
    };

    StoryExhibit::StoryExhibit() {
        m_nodeType = NodeType::Exhibit;
    }


    StoryExhibit::StoryExhibit(const StoryExhibit& storyExhibit) {

        m_nodeType = NodeType::Exhibit;
        strcpy(m_exhibitID, storyExhibit.m_exhibitID);
        strcpy(m_exhibitPrefix, storyExhibit.m_exhibitPrefix);
    }

    void to_json(json& j, const StoryExhibit& p) {
        j = json{
              {exhibitString[(int)ExhibitLayout::NodeTypeName],  nodeTypeString[(int)p.m_nodeType] },
              {exhibitString[(int)ExhibitLayout::Name],          p.m_exhibitID},
              {exhibitString[(int)ExhibitLayout::ExhibitPrefix],          p.m_exhibitPrefix}
        };
    }
    void from_json(const json& j, StoryExhibit& p) {
        p.m_nodeType = NodeType::Exhibit;
        GetCharPointerException(p.m_exhibitID, j, exhibitString[(int)ExhibitLayout::Name]);
        GetCharPointerException(p.m_exhibitPrefix, j, exhibitString[(int)ExhibitLayout::ExhibitPrefix]);
    }

}
