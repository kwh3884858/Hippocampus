#include "Example4MultiEditor.h"
#include "imnodes.h"
#include "imgui.h"
//#include <SDL_scancode.h>

#include <vector>

namespace  Example
{
    namespace Example4Namespace
    {

        const int Keycode_A = 65;

        struct Node
        {
            int id;
            float value;
        };

        struct Link
        {
            int id;
            int start_attr, end_attr;
        };

        struct Editor
        {
            imnodes::EditorContext* context = nullptr;
            std::vector<Node> nodes;
            std::vector<Link> links;
            int current_id = 0;
        };

        void show_editor(const char* editor_name, Editor& editor)
        {
            imnodes::EditorContextSet(editor.context);

            //ImGui::Begin(editor_name);
            ImGui::TextUnformatted("A -- add node");

            imnodes::BeginNodeEditor();

            for (Node& node : editor.nodes)
            {
                imnodes::BeginNode(node.id);

                imnodes::BeginNodeTitleBar();
                ImGui::TextUnformatted("node");
                imnodes::EndNodeTitleBar();

                imnodes::BeginInputAttribute(node.id << 8);
                ImGui::TextUnformatted("input");
                imnodes::EndAttribute();

                ImGui::PushItemWidth(120.0f);
                ImGui::DragFloat("value", &node.value, 0.01f);
                ImGui::PopItemWidth();

                imnodes::BeginOutputAttribute(node.id << 16);
                const float text_width = ImGui::CalcTextSize("output").x;
                ImGui::Indent(120.f + ImGui::CalcTextSize("value").x - text_width);
                ImGui::TextUnformatted("output");
                imnodes::EndAttribute();

                imnodes::EndNode();
            }

            for (const Link& link : editor.links)
            {
                imnodes::Link(link.id, link.start_attr, link.end_attr);
            }

            imnodes::EndNodeEditor();

            {
                Link link;
                if (imnodes::IsLinkCreated(&link.start_attr, &link.end_attr))
                {
                    link.id = ++editor.current_id;
                    editor.links.push_back(link);
                }
            }

            if (ImGui::IsKeyReleased(Keycode_A) &&
                ImGui::IsWindowFocused(ImGuiFocusedFlags_RootAndChildWindows))
            {
                const int node_id = ++editor.current_id;
                imnodes::SetNodeScreenSpacePos(node_id, ImGui::GetMousePos());
                editor.nodes.push_back(Node{ node_id, 0.f });
            }

            //ImGui::End();
        }

        Editor editor1;
        Editor editor2;
    } // namespace

    void Example4::NodeEditorInitialize()
    {
        Example4Namespace::editor1.context = imnodes::EditorContextCreate();
        Example4Namespace::editor2.context = imnodes::EditorContextCreate();
    }

    void Example4::NodeEditorShow()
    {
        show_editor("editor1", Example4Namespace::editor1);
        show_editor("editor2", Example4Namespace::editor2);
    }

    void Example4::NodeEditorShutdown()
    {
        imnodes::EditorContextFree(Example4Namespace::editor1.context);
        imnodes::EditorContextFree(Example4Namespace::editor2.context);
    }

} // namespace example
