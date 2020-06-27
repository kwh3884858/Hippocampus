#include "StoryJsonEndNode.h"

namespace HeavenGateEditor {

    enum class EndLayout {
        NodeTypeName = 0,
    };
    extern char endNodeString[][MAX_ENUM_LENGTH] = {
            "typename",
    };

    StoryEnd::StoryEnd()
    {
        m_nodeType = NodeType::End;
    }
    StoryEnd::StoryEnd(const StoryEnd & storyEnd)
    {
        m_nodeType = storyEnd.m_nodeType;
    }
    void to_json(json & j, const StoryEnd & p)
    {
        j = json{
          {endNodeString[(int)EndLayout::NodeTypeName],      nodeTypeString[(int)p.m_nodeType] },
        };
    }
    void from_json(const json & j, StoryEnd & p)
    {
        p.m_nodeType = NodeType::End;
    }
}
