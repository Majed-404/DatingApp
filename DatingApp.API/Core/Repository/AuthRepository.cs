using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Core.IRepository;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Core.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        
        public async Task<User> Login(string _userName, string _password)
        {
            var _user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userName);
            
            if(_user == null)
                return null;
            
            if(!VerifyPasswordHash(_password, _user.PasswordHash, _user.PasswordSalt))
                return null;

            return _user;
        }

        private bool VerifyPasswordHash(string _password, byte[] _passwordHash, byte[] _passwordSalt)
        {
            using(var _hmac = new System.Security.Cryptography.HMACSHA512(_passwordSalt))
            {
                var _computedHash = _hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(_password));
                for (int i = 0; i < _computedHash.Length; i++)
                {
                    if(_computedHash[i] != _passwordHash[i]) return false;
                }

                return true;
            } 
        }

        public async Task<User> Register(User _user, string _password)
        {
            //convert password
            byte[] _passwordHash, _passwordSalt;
            CreatePasswordHash(_password, out _passwordHash, out _passwordSalt);

            _user.PasswordHash = _passwordHash;
            _user.PasswordSalt = _passwordSalt;

            await _context.Users.AddAsync(_user);
            await _context.SaveChangesAsync();

            return _user;
        }

        private void CreatePasswordHash(string _password, out byte[] _passwordHash, out byte[] _passwordSalt)
        {
            using(var _hmac = new System.Security.Cryptography.HMACSHA512())
            {
                _passwordSalt = _hmac.Key;
                _passwordHash = _hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(_password));

            } 
        }

        public async Task<bool> UserExists(string _userName)
        {
            if (await _context.Users.AnyAsync(u=>u.UserName == _userName))
                return true;

            return false;
        }
    }
}