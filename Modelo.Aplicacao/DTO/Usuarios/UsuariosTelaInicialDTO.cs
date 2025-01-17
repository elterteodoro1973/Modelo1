﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO.Usuarios
{
    public class UsuariosTelaInicialDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public bool Administrador { get; set; }
        public Guid? CBHPrincipalId { get; set; }
    }
}
