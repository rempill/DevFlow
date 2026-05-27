using DevFlow.Domain.Enums;

namespace DevFlow.Application.Dtos;

public sealed record DiagramUploadDto(
	string FileName,
	string Content,
	VersionBumpType BumpType);

