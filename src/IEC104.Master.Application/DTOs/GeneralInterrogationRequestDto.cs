using System;
using System.Collections.Generic;
using System.Text;

namespace IEC104.Master.Application.DTOs
{
    public sealed record GeneralInterrogationRequestDto(
        byte Qoi = 20);
}