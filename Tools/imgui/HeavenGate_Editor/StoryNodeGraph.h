#pragma once
class StoryNodeGraph
{
public:
	StoryNodeGraph();
	~StoryNodeGraph();

    
};


class StoryNodeGraphBaseNode
{
public:
    StoryNodeGraphBaseNode() = delete;

    StoryNodeGraphBaseNode(unsigned int id);
    StoryNodeGraphBaseNode(unsigned int chapterId, unsigned int nodeId);

    ~StoryNodeGraphBaseNode();
    int GetChapterID() const;
    int GetNodeID() const;

private:
    static int CURRENT_ID;

    static const int NUM_NODE_ID_BIT = 0;
    static const int NUM_CHAPTER_ID_BIT = 16;

    static const int NUM_NODE_ID_SHIFT = 0;
    static const int NUM_CHAPTER_ID_SHIFT = NUM_NODE_ID_SHIFT + NUM_CHAPTER_ID_BIT;

    static const int MASK_NODE_ID = (1 << NUM_CHAPTER_ID_BIT) - 1;
    static const int MASK_CHAPTER_ID = MASK_NODE_ID << NUM_CHAPTER_ID_SHIFT;

    void UpdateCurrentID(unsigned int id);

    int m_id;

};

//
//class StoryNodeGraphNodeID
//{
//public:
//    StoryNodeGraphNodeID();
//    ~StoryNodeGraphNodeID();
//
//    static  int GetNewID();
//private:
//    static int m_currentId;
//};
//
//StoryNodeGraphNodeID::StoryNodeGraphNodeID()
//{
//}
//
//StoryNodeGraphNodeID::~StoryNodeGraphNodeID()
//{
//}
