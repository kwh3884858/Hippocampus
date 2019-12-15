#include "StoryJson.h"
#include <string>
using std::string;

namespace HeavenGateEditor {
    //using NodeType = StoryNode::NodeType;

    enum class LabelLayout {
        NodeTypeName = 0,
        Label = 1
    };

    enum class JumpLayout
    {
        NodeTypeName = 0,
        Jump = 1
    };

    enum class WordLayout {
        NodeTypeName = 0,
        Name,
        Content
    };

    char nodeTypeString[][MAX_ENUM_LENGTH] = {
        "none",
        "label",
        "word",
        "jump"
    };

    extern char labelNodeString[][MAX_ENUM_LENGTH] = {
        "typename",
        "label"
    };
    extern char jumpNodeString[][MAX_ENUM_LENGTH] = {
        "typename",
        "jump"
    };
    extern char wordNodeString[][MAX_ENUM_LENGTH] = {
        "typename",
        "name",
        "content"
    };

    int HeavenGateEditor::StoryJson::AddNode(StoryNode* const node) {
        if (node->m_nodeType == NodeType::None) {
            return -1;
        }

        m_nodes.push_back(node);
        return static_cast<int>(m_nodes.size());
    }

    int HeavenGateEditor::StoryJson::AddWord(StoryWord * const word)
    {
        m_nodes.push_back(word);
        return static_cast<int>(m_nodes.size());
    }

    int  HeavenGateEditor::StoryJson::AddWord(const char* name, const char* content) {
        StoryWord* word = new StoryWord;
        word->m_nodeType = NodeType::Word;
        strcpy(word->m_name, name);
        strcpy(word->m_content, content);

        return  AddNode(word);
    }

    int HeavenGateEditor::StoryJson::AddLabel(const char* labelName) {
        StoryLabel* label = new StoryLabel;
        label->m_nodeType = NodeType::Label;
        strcpy(label->m_labelId, labelName);

        return  AddNode(label);
    }
    int HeavenGateEditor::StoryJson::AddJump(const char* jumpName) {
        StoryJump* jump = new StoryJump;
        jump->m_nodeType = NodeType::Jump;
        strcpy(jump->m_jumpId, jumpName);

        return  AddNode(jump);
    }

    const HeavenGateEditor::StoryNode * const HeavenGateEditor::StoryJson::GetNode(int index) const
    {
        return m_nodes[index];
    }

    HeavenGateEditor::StoryNode * const HeavenGateEditor::StoryJson::GetNode(int index)
    {
        return const_cast<StoryNode*>(
            static_cast<const StoryJson&>(*this).GetNode(index));
    }

    int HeavenGateEditor::StoryJson::Size() const
    {
        return  static_cast<int>(m_nodes.size());
    }

    void StoryJson::SetFullPath(const char* fullPath) {
        strcpy(m_fullPath, fullPath);
    }
    const char* StoryJson::GetFullPath() const
{
        return m_fullPath;
    }

    bool StoryJson::IsExistFullPath()const {
        return m_fullPath != nullptr;
    }

    void to_json(json & j, const StoryWord & p)
    {
        j = json{
            {wordNodeString[(int)WordLayout::NodeTypeName],  nodeTypeString[(int)p.m_nodeType] },
            {wordNodeString[(int)WordLayout::Name],          p.m_name},
            {wordNodeString[(int)WordLayout::Content],       p.m_content }
        };

    }
    void to_json(json& j, const StoryLabel& p)
    {
        j = json{
            {labelNodeString[(int)LabelLayout::NodeTypeName],    nodeTypeString[(int)p.m_nodeType] },
            {labelNodeString[(int)LabelLayout::Label],       p.m_labelId}
        };
    }

    void to_json(json& j, const StoryJump& p)
    {
        j = json{
            {jumpNodeString[(int)JumpLayout::NodeTypeName],      nodeTypeString[(int)p.m_nodeType] },
            {jumpNodeString[(int)JumpLayout::Jump],          p.m_jumpId}

        };
    }
    void to_json(json & j, const StoryJson & story)
    {
        for (int i = 0; i < story.Size(); i++)
        {
            const StoryNode* const pNode = story.GetNode(i);

            if (pNode->m_nodeType == NodeType::Word)
            {
                const StoryWord*const pWord = static_cast<const StoryWord*const>(pNode);
                j.push_back(*pWord);
            }

            if (pNode->m_nodeType == NodeType::Label)
            {
                const StoryLabel*const pLabel = static_cast<const StoryLabel*const>(pNode);
                j.push_back(*pLabel);
            }

            if (pNode->m_nodeType == NodeType::Jump)
            {
                const StoryJump*const pJump = static_cast<const StoryJump*const>(pNode);
                j.push_back(*pJump);
            }
        }
    }


    /* String version, Archieved */

    //void from_json(const json & j, StoryWord & p)
    //{
    //    j.at("name").get_to(p.m_name);
    //    j.at("content").get_to(p.m_content);
    //}


    void from_json(const json & j, StoryWord & p)
    {
        p.m_nodeType = NodeType::Word;
        strcpy(p.m_name, j.at(wordNodeString[(int)WordLayout::Name]).get_ptr<const json::string_t *>()->c_str());
        strcpy(p.m_content, j.at(wordNodeString[(int)WordLayout::Content]).get_ptr<const json::string_t *>()->c_str());
    }

    void from_json(const json& j, StoryLabel& p) {
        p.m_nodeType = NodeType::Label;
        strcpy(p.m_labelId, j.at(labelNodeString[(int)LabelLayout::Label]).get_ptr<const json::string_t *>()->c_str());

    }
    void from_json(const json& j, StoryJump& p) {
        p.m_nodeType = NodeType::Jump;
        strcpy(p.m_jumpId, j.at(jumpNodeString[(int)JumpLayout::Jump]).get_ptr<const json::string_t *>()->c_str());
    }

    void from_json(const json & j, StoryJson & p)
    {
        for (int i = 0; i < j.size(); i++)
        {
            char enumString[MAX_ENUM_LENGTH];
            strcpy(enumString, j[i].at(wordNodeString[(int)WordLayout::NodeTypeName]).get_ptr<const json::string_t *>()->c_str());

            if (strcmp(enumString, nodeTypeString[(int)NodeType::Label]) == 0) {
                StoryLabel* node = new StoryLabel;
                *node = j[i];
                p.AddNode(node);
            }

            if (strcmp(enumString, nodeTypeString[(int)NodeType::Jump]) == 0) {
                StoryJump* node = new StoryJump;
                *node = j[i];
                p.AddNode(node);
            }

            if (strcmp(enumString, nodeTypeString[(int)NodeType::Word]) == 0) {
                StoryWord* node = new StoryWord;
                *node = j[i];
                p.AddNode(node);
            }


        }
        // iterate the array
       /* for (json::iterator it = j.begin(); it != j.end(); ++it) {
            StoryWord* word = new StoryWord();
            *word = *it;
            p.AddWord(word);
        }*/
    }


    StoryNode::StoryNode()
    {
        m_nodeType = NodeType::None;
    }

    StoryLabel::StoryLabel()
    {
        m_nodeType = NodeType::Label;
    }

    StoryJump::StoryJump()
    {
        m_nodeType = NodeType::Jump;
    }

    StoryWord::StoryWord()
    {
        m_nodeType = NodeType::Word;
    }

}
