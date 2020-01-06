#ifndef StoryJson_h
#define StoryJson_h

#include <stdio.h>

#include <list>
#include "nlohmann/json.hpp"
#include "HeavenGateEditorConstant.h"


namespace HeavenGateEditor {
    using json = nlohmann::json;
    using std::list;



    enum class NodeType
    {
        None = 0,
        Label,
        Word,
        Jump
    };
    enum class StructLayout :int;
    enum class InfoLayout :int;
    enum class LabelLayout :int;
    enum class JumpLayout :int;
    enum class WordLayout :int;


    extern char StructString[][MAX_ENUM_LENGTH];
    extern char infoString[][MAX_ENUM_LENGTH];

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
        int InsertWord(const char* name, const char* content, int index);
        int AddLabel(const char* labelName);
        int InsertLabel(const char* labelName, int index);
        int AddJump(const char* jumpName, const char* jumpContent);
        int InsertJump(const char* jumpName, const char* jumpContent, int index);

        void Swap(int lhs, int rhs);

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

        void SetChapter(const char* chapter) { strcpy(m_chapter, chapter); }
        const char *const GetChapter()const { return m_chapter; }
        char* GetChapter() { return const_cast<char*>(const_cast<const StoryJson*>(this)->GetChapter()); }

        void SetScene(const char* scene) { strcpy(m_scene, scene); }
        const char* const GetScene()const { return m_scene; }
        char* GetScene() { return const_cast<char*>(const_cast<const StoryJson*>(this)->GetScene()); }

    private:
        list<StoryNode*> m_nodes;
        char m_fullPath[MAX_FOLDER_PATH];

        char m_chapter[MAX_FOLDER_PATH];
        char m_scene[MAX_FOLDER_PATH];

    };

    void GetContentException(char*const des, const json & j, const char* const index);
    void GetJsonException(json & des, const json& src, const char* const index);

    void to_json(json& j, const StoryWord& p);
    void to_json(json& j, const StoryJson& p);
    void to_json(json& j, const StoryLabel& p);
    void to_json(json& j, const StoryJump& p);

    void from_json(const json& j, StoryWord& p);
    void from_json(const json& j, StoryJson& p);
    void from_json(const json& j, StoryLabel& p);
    void from_json(const json& j, StoryJump& p);


    void ToJsonFactory(json& j, const StoryJson& p);
    void FromJsonFactory(const json& j, StoryJson & p);

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
