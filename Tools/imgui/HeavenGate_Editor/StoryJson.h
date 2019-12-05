#ifndef StoryJson_h
#define StoryJson_h

#include <stdio.h>

#include <vector>
#include "nlohmann/json.hpp"
#include "HeavenGateEditorConstant.h"


using json = nlohmann::json;
using std::string;
using std::vector;
namespace HeavenGateEditor {
    enum NodeType
    {
        None = 0,
        Label,
        Word,
        Jump
    };

    char nodeTypeString[][MAX_ENUM_LENGTH]={"none","label","word","jump"};


    class StoryNode {
    public:
        NodeType m_nodeType;
    };

    class StoryLabel :public StoryNode
    {
    public:
        char m_labelId[MAX_ID];
    };

    class StoryJump : public StoryNode
    {
    public:
        char m_jumpId[MAX_ID];

    private:

    };

    class StoryWord :public StoryNode {
    public:

        char m_name[MAX_NAME];
        char m_content[MAX_CONTENT];


    };

    class StoryJson {
    private:
        vector<StoryNode*> m_nodes;

    public:
                int AddNode(StoryNode* const node);

        int AddWord(StoryWord* const word);
        int AddWord(const char* name, const char* content);
        int AddLabel(const char* labelName);
        int AddJump(const char* jumpName);

        void SetWord(const StoryWord* const word);

        StoryNode* const GetNode(int index);
        const StoryNode* const GetNode(int index) const;

        void RemoveWord();
        StoryWord FindWord(int index)const;
        int Size() const;
        void Clear();
        bool Empty()const;

        void SetFullPath(const char* fullPath);
        const char* GetFullPath();
        bool IsExistFullPath()const;

    private:
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
