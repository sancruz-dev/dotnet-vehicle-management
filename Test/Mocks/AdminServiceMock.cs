using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;

namespace Test.Mocks
{
    public class AdminServiceMock : IAdminService
    {
        private static List<Admin> listaAdmins = new List<Admin>() { 
            new Admin
            {
                Id = 1,
                Email = "mockAdm@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            },
            new Admin
            {
                Id = 2,
                Email = "mockEditor@teste.com",
                Senha = "123456",
                Perfil = "Editor"
            }
        };

        public Admin? BuscaPorId(int id)
        {
            return listaAdmins.Find(a => a.Id == id);
        }

        public Admin Incluir(Admin admin)
        {   
            admin.Id = listaAdmins.Count() + 1;
            listaAdmins.Add(admin);

            return admin;
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            return listaAdmins.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
        }

        public List<Admin> Todos(int? pagina)
        {
            return listaAdmins;
        }
    }
}
