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
    "jump"
};

StoryNode::StoryNode()
{
    m_nodeType = NodeType::None;
}

StoryNode::StoryNode(const StoryNode& storyNode)
{
    m_nodeType = storyNode.m_nodeType;

}

void GetContentException(char*const des, const json & j, const char* const index) {

    try
    {
        strcpy(des, j.at(index).get_ptr<const json::string_t *>()->c_str());
    }
    catch (json::exception& e)
    {
        printf("message: %s \n exception id: %d \n lack of: %s \n\n", e.what(), e.id, index);

        memset(des, '\0', sizeof(des));
    }

}

void GetJsonException(json & des, const json& src, const char* const index) {
    try
    {
        des = src.at(index);
    }
    catch (json::exception& e)
    {
        printf("message: %s \n exception id: %d \n lack of: %s \n\n", e.what(), e.id, index);

        des = json();
    }

}


}
