// Painter.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "../libpainter/Designer.h"
#include "../libpainter/ShapeFactory.h"
#include "../libpainter/PictureDraft.h"
#include "../libpainter/Painter.h"
#include "../libpainter/Canvas.h"

using namespace std;

int main()
{
	unique_ptr<IShapeFactory> factory = make_unique<CShapeFactory>();
	unique_ptr<IDesigner> designer = make_unique<CDesigner>(*factory);
	auto draft = designer->CreateDraft(cin);
	unique_ptr<IPainter> painter = make_unique<CPainter>();
	unique_ptr<ICanvas> canvas = make_unique<CCanvas>(cout);
	painter->DrawPicture(draft, *canvas);

	return 0;
}
