using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedeSocial.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeSocial.DOMAIN;


public class RedeSocialContext : IdentityDbContext<IdentityUser>
{
    public RedeSocialContext(DbContextOptions<RedeSocialContext> options) : base(options) { }

   
    public DbSet<ProfileUser> profileUsers { get; set; }

    public DbSet<Post> posts{ get; set; }

    public DbSet<Users> users { get; set; }
}