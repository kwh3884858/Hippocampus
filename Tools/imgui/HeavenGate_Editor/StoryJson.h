#ifndef StoryJson_h
#define StoryJson_h

#include <stdio.h>

#include <vector>
#include "nlohmann/json.hpp"
#include "HeavenGateEditorConstant.h"


namespace HeavenGateEditor {
    using json = nlohmann::json;
    using std::vector;



    enum class NodeType
    {
        None = 0,
        Label,
        Word,
        Jump
    };
    enum class LabelLayout :int;
    enum class JumpLayout :int;
    enum class WordLayout :int;

    extern char nodeTypeString[][MAX_ENUM_LENGTH];
    extern char labelNodeString[][MAX_ENUM_LENGTH];
    extern char jumpNodeString[][MAX_ENUM_LENGTH];
    extern char wordNodeString[][MAX_ENUM_LENGTH];

    class StoryNode {
    public:

        NodeType m_nodeType;

        StoryNode();
        StoryNode(const StoryNode& storyNode);
    };

    class StoryLabel :public StoryNode
    {
    public:
        char m_labelId[MAX_ID];

        StoryLabel();
        StoryLabel(const StoryLabel& storyLabel);
    };

    class StoryJump : public StoryNode
    {
    public:
        char m_jumpId[MAX_ID];
        char m_jumpContent[MAX_CONTENT];

        StoryJump();
        StoryJump(const StoryJump& storyJump);
    };

    class StoryWord :public StoryNode {
    public:

        char m_name[MAX_NAME];
        char m_content[MAX_CONTENT];

        StoryWord();
        StoryWord(const StoryWord& storyWard);
    };

    class StoryJson {

    public:
        StoryJson();
        StoryJson(const StoryJson& storyJson);
        StoryJson( StoryJson&& storyJson)noexcept;
        ~StoryJson();

        StoryJson& operator=(StoryJson&& storyJson)noexcept;

        int AddNode(StoryNode* const node);

        int AddWord(StoryWord* const word);
        int AddWord(const char* name, const char* content);
        int AddLabel(const char* labelName);
        int AddJump(const char* jumpName, const char* jumpContent);


        StoryNode* const GetNode(int index);
        const StoryNode* const GetNode(int index) const;

        void RemoveWord();
        StoryWord FindWord(int index)const;
        int Size() const;
        void Clear();
        bool Empty()const;

        void SetFullPath(const char* fullPath);
        const char* GetFullPath()const;
        bool IsExistFullPath()const;

    private:
        vector<StoryNode*> m_nodes;
        char m_fullPath[MAX_FOLDER_PATH];
    };

    void to_json(json& j, const StoryWord& p);
    void to_json(json& j, const StoryJson& p);
    void to_json(json& j, const StoryLabel& p);
    void to_json(json& j, const StoryJump& p);

    void from_json(const json& j, StoryWord& p);
    void from_json(const json& j, StoryJson& p);
    void from_json(const json& j, StoryLabel& p);
    void from_json(const json& j, StoryJump& p);


    //void to_json(json& j, const StoryWord* p) {
    //    j = json{ {"name", p->m_name}, {"content", p->m_content} };
    //}

    //void from_json(const json& j, StoryWord* p) {
    //    j.at("name").get_to(p.m_name);
    //    j.at("content").get_to(p.m_content);
    //}
    //void to_json(json& j, const StoryJson& p){
    //    for (int i = 0 ; i < p.m_nodes.size(); i++) {
    //
    //    }
    //}
    //
    // void from_json(const json& j, StoryJson& p) {
    //
    //     }
}

#endif /* StoryJson_h */
