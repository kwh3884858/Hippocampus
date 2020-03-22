//
//  StoryJsonStoryNode.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#ifndef StoryJsonStoryNode_h
#define StoryJsonStoryNode_h

#include "nlohmann/json.hpp"
#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {
using json = nlohmann::json;

enum class NodeType
{
    None = 0,
    Label,
    Word,
    Jump,
    Exhibit
};
extern char nodeTypeString[][MAX_ENUM_LENGTH];

class StoryNode {
public:

    NodeType m_nodeType;

    StoryNode();
    StoryNode(const StoryNode& storyNode);
};



void GetContentException(char*const des, const json & j, const char* const index);
void GetJsonException(json & des, const json& src, const char* const index);


}

#endif /* StoryJsonStoryNode_h */
