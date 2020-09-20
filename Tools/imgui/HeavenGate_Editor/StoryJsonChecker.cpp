#include "StoryJsonChecker.h"

#include "StoryJson.h"
#include "StoryJsonStoryNode.h"
#include "StoryJsonWordNode.h"
#include "StoryJsonLabelNode.h"
#include "StoryJsonJumpNode.h"

#include <vector>

namespace HeavenGateEditor {

    using std::vector;



    bool StoryJsonChecker::CheckJsonStory(const StoryJson* const story, int& errorIndex) const
    {
        bool result = true;
        result = CheckLabelAndJumpPosition(story, errorIndex);
        if (!result)
        {
            printf("Label and Jump position is illegal \n");
            return false;
        }

        return true;

    }

    bool StoryJsonChecker::CheckLabelAndJumpPosition(const StoryJson* const story, int& errorIndex) const
    {
        vector<const StoryLabel* > readedList;
        for (int i = 0; i < story->Size(); i++)
        {
            const StoryNode* const node = story->GetNode(i);
            if (node->m_nodeType == NodeType::Label)
            {
                const StoryLabel* const label = static_cast<const StoryLabel*const>(node);
                readedList.push_back(label);
            }
            else if (node->m_nodeType == NodeType::Jump)
            {
                const StoryJump*const jump = static_cast<const StoryJump*const>(node);
                if (!readedList.empty())
                {
                    for (auto iter = readedList.cbegin(); iter != readedList.cend(); iter++)
                    {
                        if (strcmp((*iter)->m_labelId, jump->m_jumpId) == 0)
                        {
                            errorIndex = i;
                            return false;
                        }
                    }
                }
            }
            else
            {
                readedList.clear();
            }
        }

        return true;
    }

    bool StoryJsonChecker::CheckJsonNameAndContentlengthLimit(const StoryJson* const story, int& errorIndex) const
    {
        for (int i = 0; i < story->Size(); i++)
        {
            const StoryNode* const node = story->GetNode(i);
            if (node->m_nodeType == NodeType::Word)
            {
                const StoryWord*const word = static_cast<const StoryWord*const>(node);
                if (strlen(word->m_name) >= MAX_NAME_LIMIT) {
                    errorIndex = i;
                    return false;
                }
                if (strlen(word->m_content) >= MAX_CONTENT_LIMIT)
                {
                    errorIndex = i;
                    return false;
                }
            }
        }
        return true;
    }

}
