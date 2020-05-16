#include "StoryNodeGraph.h"

#include <assert.h>

StoryNodeGraph::StoryNodeGraph()
{
}


StoryNodeGraph::~StoryNodeGraph()
{
}


int StoryNodeGraphBaseNode::CURRENT_ID = 0;

void StoryNodeGraphBaseNode::UpdateCurrentID(unsigned int id)
{
    int nodeId = MASK_NODE_ID & id;
    assert(nodeId > 0 && nodeId < (1 << NUM_NODE_ID_SHIFT));

    CURRENT_ID = nodeId;
}

StoryNodeGraphBaseNode::StoryNodeGraphBaseNode(unsigned int id)
{
    m_id = id;
}

StoryNodeGraphBaseNode::StoryNodeGraphBaseNode(unsigned int chapterId, unsigned int nodeId)
{
    assert(chapterId > 0 && chapterId < (1 << NUM_CHAPTER_ID_BIT));
    assert(nodeId > 0 && nodeId < (1 << NUM_NODE_ID_SHIFT));

    m_id = CURRENT_ID;
    CURRENT_ID++;

    m_id = (chapterId << NUM_CHAPTER_ID_SHIFT) | m_id;
}

StoryNodeGraphBaseNode::~StoryNodeGraphBaseNode()
{
}

int StoryNodeGraphBaseNode::GetChapterID() const
{
    return MASK_CHAPTER_ID & m_id;
}

int StoryNodeGraphBaseNode::GetNodeID() const
{
    return MASK_NODE_ID & m_id;
}



