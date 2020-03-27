#include "HeavenGateEditorNodeGraphExample.h"

#include "Example1Simple.h"
#include "Example2SaveLoadEditor.h"
#include "Example3ColorNodeEditor.h"
#include "Example4MultiEditor.h"


namespace HeavenGateEditor {

    HeavenGateEditorNodeGraphExample::HeavenGateEditorNodeGraphExample()
    {
        m_isInitialization = false;
    }


    HeavenGateEditorNodeGraphExample::~HeavenGateEditorNodeGraphExample()
    {
        Shutdown();
    }

    void HeavenGateEditor::HeavenGateEditorNodeGraphExample::UpdateMainWindow()
    {
        if (m_isInitialization == false) {
            Initialization();
        }

        Example1::NodeEditorShow();
//        Example2::NodeEditorShow();
//        Example3::NodeEditorShow();
//        Example4::NodeEditorShow();
    }

    void HeavenGateEditor::HeavenGateEditorNodeGraphExample::UpdateMenu()
    {

    }

void HeavenGateEditorNodeGraphExample::Initialization(){
    Example1::NodeEditorInitialize();
//       Example2::NodeEditorInitialize();
//       Example3::NodeEditorInitialize();
//       Example4::NodeEditorInitialize();
}
void HeavenGateEditorNodeGraphExample::Shutdown(){
    Example1::NodeEditorShutdown();
//        Example2::NodeEditorShutdown();
//        Example3::NodeEditorShutdown();
//        Example4::NodeEditorShutdown();
}

}
