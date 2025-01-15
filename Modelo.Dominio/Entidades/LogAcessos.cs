﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Modelo.Dominio.Entidades
{
    public partial class LogAcessos : EntidadeBase
    {
        
        [Column(TypeName = "datetime")]
        public DateTime Data { get; set; }
        public Guid? UsuarioId { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Comando { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string Entidades { get; set; }
        
        public bool? LogTransacoes { get; set; }

        [ForeignKey("UsuarioId")]
        [InverseProperty("LogAcessos")]
        public virtual Usuarios Usuario { get; set; }
    }
}