#pragma once

#include "IDesigner.h"

struct IShapeFactory;

class CDesigner : public IDesigner
{
public:
	CDesigner(IShapeFactory & factory);
	CPictureDraft CreateDraft(std::istream & inputData) final;
private:
	IShapeFactory & m_factory;
};
