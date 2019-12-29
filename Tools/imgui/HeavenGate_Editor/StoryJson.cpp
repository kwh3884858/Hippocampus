#include "StoryJson.h"

#include <utility>

#include <stdexcept>


namespace HeavenGateEditor {
    //using NodeType = StoryNode::NodeType;

    enum class LabelLayout {
        NodeTypeName = 0,
        Label = 1
    };

    enum class JumpLayout
    {
        NodeTypeName = 0,
        Jump = 1,
        Content
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
        "jump",
        "content"
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
    int HeavenGateEditor::StoryJson::AddJump(const char* jumpName, const char* jumpContent) {
        StoryJump* jump = new StoryJump;
        jump->m_nodeType = NodeType::Jump;
        strcpy(jump->m_jumpId, jumpName);
        strcpy(jump->m_jumpContent, jumpContent);

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

    StoryJson::StoryJson()
    {
        memset(m_fullPath, 0, sizeof(m_fullPath));

    }

    StoryJson::StoryJson(const StoryJson& storyJson)
    {
        if (this == &storyJson)
        {
            return;
        }
        memset(m_fullPath, 0, sizeof(m_fullPath));
        strcpy(m_fullPath, storyJson.m_fullPath);

        //m_nodes = storyJson.m_nodes;
        for (int i = 0; i < storyJson.m_nodes.size(); i++)
        {
            
            if (storyJson.m_nodes[i]->m_nodeType == NodeType::Word)
            {
                StoryWord* word = new StoryWord(static_cast<const StoryWord&>(*(storyJson.m_nodes[i])));
                m_nodes.push_back(word);
            }
            else if(storyJson.m_nodes[i]->m_nodeType == NodeType::Jump)
            {
                StoryJump* jump = new StoryJump(static_cast<const StoryJump&>(*(storyJson.m_nodes[i])));
                m_nodes.push_back(jump);

            }
            else if (storyJson.m_nodes[i]->m_nodeType == NodeType::Label)
            {
                StoryLabel* label = new StoryLabel(static_cast<const StoryLabel&>(*(storyJson.m_nodes[i])));
                m_nodes.push_back(label);

            }
        }
    }

    StoryJson::StoryJson(StoryJson&& storyJson) noexcept
    {
        if (this == &storyJson)
        {
            return;
        }
        memset(m_fullPath, 0, sizeof(m_fullPath));
        strcpy(m_fullPath, storyJson.m_fullPath);

        m_nodes = storyJson.m_nodes;

        for (int i = 0; i < storyJson.m_nodes.size(); i++)
        {
            storyJson.m_nodes[i] = nullptr;
        }
    }

    StoryJson::~StoryJson()
    {
        Clear();
    }

    StoryJson& StoryJson::operator=(StoryJson&& storyJson) noexcept
    {
        if (this == &storyJson) {
            return *this;
        }
        Clear();
        memset(m_fullPath, 0, sizeof(m_fullPath));
        strcpy(m_fullPath, storyJson.m_fullPath);
        m_nodes = storyJson.m_nodes;

        for (int i = 0; i < storyJson.m_nodes.size(); i++)
        {
            storyJson.m_nodes[i] = nullptr;
        }

        return *this;
    }

    void StoryJson::Clear()
    {
        for (int i = 0; i < m_nodes.size(); i++)
        {
            if (m_nodes[i] == nullptr)
            {
                continue;
            }
            delete m_nodes[i];
            m_nodes[i] = nullptr;
        }
        m_nodes.clear();


        memset(m_fullPath, 0, sizeof(m_fullPath));
    }

    void StoryJson::SetFullPath(const char* fullPath) {
        strcpy(m_fullPath, fullPath);
    }
    const char* StoryJson::GetFullPath() const
    {
        return m_fullPath;
    }

    bool StoryJson::IsExistFullPath()const {
        return strlen(m_fullPath) != 0;
    }


    //=========================DATA STRUCTURE============================


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
            {jumpNodeString[(int)JumpLayout::Jump],          p.m_jumpId},
            {jumpNodeString[(int)JumpLayout::Content],          p.m_jumpContent}

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

    //=========================Exception===========================
    void GetContentException(char*const des, const json & j,const char* const index) {

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
        GetContentException(p.m_jumpId, j, jumpNodeString[(int)JumpLayout::Jump]);
        GetContentException(p.m_jumpContent, j, jumpNodeString[(int)JumpLayout::Content]);
        /*      strcpy(p.m_jumpId, j.at(jumpNodeString[(int)JumpLayout::Jump]).get_ptr<const json::string_t *>()->c_str());
              strcpy(p.m_jumpContent, j.at(jumpNodeString[(int)JumpLayout::Content]).get_ptr<const json::string_t *>()->c_str());*/
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

    StoryNode::StoryNode(const StoryNode& storyNode)
    {
        m_nodeType = storyNode.m_nodeType;

    }

    StoryLabel::StoryLabel()
    {
        m_nodeType = NodeType::Label;
    }

    StoryLabel::StoryLabel(const StoryLabel& storyLabel)
    {
        m_nodeType = storyLabel.m_nodeType;
        strcpy(m_labelId, storyLabel.m_labelId);
    }

    StoryJump::StoryJump()
    {
        m_nodeType = NodeType::Jump;
    }

    StoryJump::StoryJump(const StoryJump& storyJump)
    {
        m_nodeType = storyJump.m_nodeType;
        strcpy(m_jumpId, storyJump.m_jumpId);
        strcpy(m_jumpContent, storyJump.m_jumpContent);

    }

    StoryWord::StoryWord()
    {
        m_nodeType = NodeType::Word;
    }

    StoryWord::StoryWord(const StoryWord& storyWard)
    {
        m_nodeType = storyWard.m_nodeType;
        strcpy(m_name, storyWard.m_name);
        strcpy(m_content, storyWard.m_content);

    }

}
