using System.Security.Authentication;
using NHibernate;
using Pizza.Contracts.Security;
using Pizza.Contracts.Security.ServiceContracts;
using Pizza.Contracts.Security.ViewModels;
using Pizza.Framework.Persistence.Transactions;
using Pizza.Framework.ValueInjection;
using Pizza.Persistence;
using Pizza.Persistence.Default;
using Pizza.Utils;

namespace Pizza.Framework.Security
{
    [Transactional]
    public abstract class AuthServiceBase<TUser> : IAuthService
        where TUser : class, IPizzaUser, IPersistenceModel // TODO: remove IPizzaUser to allow use really any persistence model to be used?
    {
        protected readonly ISession session;

        protected AuthServiceBase(ISession session)
        {
            this.session = session;
        }

        public LoginResult<TUserViewModel> LoginUser<TUserViewModel>(LoginRequest loginRequest)
            where TUserViewModel : PizzaUserViewModel, new()
        {
            this.session.Clear();

            var user = this.GetUserByName(loginRequest.UserName);

            if (user == null || user.Password != HashGenerator.Generate(loginRequest.Password))
            {
                return new LoginResult<TUserViewModel>();
            }

            var userViewModel = Injector.CreateViewModelFromPersistenceModel<TUser, TUserViewModel>(user);
            return new LoginResult<TUserViewModel>(userViewModel);
        }

        public ChangePasswordResult ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var user = this.GetUserByName(changePasswordRequest.UserName);
            if (user.Password != HashGenerator.Generate(changePasswordRequest.OldPassword))
            {
                return new ChangePasswordResult("Invalid username or password");
            }

            // TODO: provide way to check password strength

            user.Password = HashGenerator.Generate(changePasswordRequest.NewPassword);
            this.session.Save(user);
            return new ChangePasswordResult();
        }

        private TUser GetUserByName(string username)
        {
            var user = this.GetUser(username);

            if (user == null)
            {
                throw new AuthenticationException($"User with name: {username} does not exist");
            }

            return user;
        }

        protected abstract TUser GetUser(string username);
       
    }
}