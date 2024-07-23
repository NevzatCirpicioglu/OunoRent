using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityLayer.Entities;

public class Slider : AuditTrailer
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string ImgUrl { get; set; }

    public string TargetUrl { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; }

}
