#include "Example1Simple.h"
#include "imnodes.h"
#include "imgui.h"

#include <unordered_map>
#include <utility>

namespace  Example
    {
    namespace Example1Namespace
        {
        class SimpleNodeEditor
        {
        public:
            void show()
            {
                //ImGui::Begin("simple node editor");

                imnodes::BeginNodeEditor();
                imnodes::BeginNode(1);

                imnodes::BeginNodeTitleBar();
                ImGui::TextUnformatted("simple node :)");
                imnodes::EndNodeTitleBar();

                imnodes::BeginInputAttribute(2);
                ImGui::Text("input");
                imnodes::EndAttribute();

                imnodes::BeginOutputAttribute(3);
                ImGui::Indent(40);
                ImGui::Text("output");
                imnodes::EndAttribute();

                imnodes::EndNode();
                imnodes::EndNodeEditor();

                //ImGui::End();
            }
        };

        static SimpleNodeEditor editor;
        } // namespace

    void Example1::NodeEditorInitialize()
    {
        imnodes::SetNodeGridSpacePos(1, ImVec2(200.0f, 200.0f));
    }

    void Example1::NodeEditorShow() { Example1Namespace::editor.show(); }

    void Example1::NodeEditorShutdown() {}

} // namespace example
