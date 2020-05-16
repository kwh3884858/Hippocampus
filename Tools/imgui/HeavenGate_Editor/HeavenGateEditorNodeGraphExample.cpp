#include "HeavenGateEditorNodeGraphExample.h"

#include "Example1Simple.h"
#include "Example2SaveLoadEditor.h"
#include "Example3ColorNodeEditor.h"
#include "Example4MultiEditor.h"

#include "imgui.h"

namespace HeavenGateEditor {

    HeavenGateEditorNodeGraphExample::HeavenGateEditorNodeGraphExample()
    {

    }


    HeavenGateEditorNodeGraphExample::~HeavenGateEditorNodeGraphExample()
    {
        
    }

    void HeavenGateEditorNodeGraphExample::Initialize()
    {
        Example::Example1* example1 = new Example::Example1;
        Example::Example2* example2 = new Example::Example2;
        Example::Example3* example3 = new Example::Example3;
        Example::Example4* example4 = new Example::Example4;


        m_exampleArray[0] = example1;
        m_exampleArray[1] = example2;
        m_exampleArray[2] = example3;
        m_exampleArray[3] = example4;

        m_currentExample = m_exampleArray[2];
        m_currentExample->NodeEditorInitialize();
        //Example1::NodeEditorInitialize();
                //Example2::NodeEditorInitialize();
        //       Example3::NodeEditorInitialize();
        //       Example4::NodeEditorInitialize();
    }

    void HeavenGateEditorNodeGraphExample::Shutdown()
    {
        //Example1::NodeEditorShutdown();
                //Example2::NodeEditorShutdown();
        //        Example3::NodeEditorShutdown();
        //        Example4::NodeEditorShutdown();

        m_currentExample->NodeEditorShutdown();

        delete[] m_exampleArray;
    }

    void HeavenGateEditorNodeGraphExample::OpenExample(int index)
    {
        m_currentExample->NodeEditorShutdown();
        m_currentExample = m_exampleArray[index];
        m_currentExample->NodeEditorInitialize();

    }

    void HeavenGateEditor::HeavenGateEditorNodeGraphExample::UpdateMainWindow()
    {

        m_currentExample->NodeEditorShow();

        //Example1::NodeEditorShow();
        //Example2::NodeEditorShow();
//        Example3::NodeEditorShow();
//        Example4::NodeEditorShow();
    }

    void HeavenGateEditor::HeavenGateEditorNodeGraphExample::UpdateMenu()
    {
        if (ImGui::MenuItem("Example1")) {
            OpenExample(0);
        }
        if (ImGui::MenuItem("Example2")) {
            OpenExample(1);

        }
        if (ImGui::MenuItem("Example3")) {
            OpenExample(2);

        }
        if (ImGui::MenuItem("Example4")) {
            OpenExample(3);

        }
    }


}
