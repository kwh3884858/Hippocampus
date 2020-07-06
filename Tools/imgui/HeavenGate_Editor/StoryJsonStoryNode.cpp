//
//  StoryJsonStoryNode.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#include "StoryJsonStoryNode.h"


namespace HeavenGateEditor {

extern char nodeTypeString[][MAX_ENUM_LENGTH] = {
    "none",
    "label",
    "word",
    "jump",
    "exhibit",
    "end"
};

StoryNode::StoryNode()
{
    m_nodeType = NodeType::None;
}

StoryNode::StoryNode(const StoryNode& storyNode)
{
    m_nodeType = storyNode.m_nodeType;
}

}
