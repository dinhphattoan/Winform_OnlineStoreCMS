using OnlineStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Helpers
{
    public class ToastModel
    {
        public string? Message { get; set; }
        public ToastType? MessageType { get; set; }
    }
}
