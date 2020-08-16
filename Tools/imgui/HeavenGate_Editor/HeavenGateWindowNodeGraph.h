#pragma once


#include "HeavenGateEditorBaseWindow.h"


namespace HeavenGateEditor {

    class HeavenGateWindowNodeGraph : public HeavenGateEditorBaseWindow
    {

        WINDOW_DECLARE("Heaven Gate Editor", Window_Type::MainWindow)
    public:
//        HeavenGateWindowNodeGraph();
        HeavenGateWindowNodeGraph(HeavenGateEditorBaseWindow* parent);

        virtual ~HeavenGateWindowNodeGraph() override;


        virtual void Initialize() override{}
        virtual void Shutdown() override{}
        virtual void UpdateMainWindow() override{}
        virtual void UpdateMenu() override {}


    private:

    };

}
