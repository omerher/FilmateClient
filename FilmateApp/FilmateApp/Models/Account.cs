using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FilmateApp.Models
{
    [Table("Account")]
    [Index(nameof(Email), Name = "I_Account_Email", IsUnique = true)]
    [Index(nameof(Username), Name = "I_Account_Username", IsUnique = true)]
    [Index(nameof(Salt), Name = "UQ__Account__A152BCCE5793FBF5", IsUnique = true)]
    public partial class Account
    {
        public Account()
        {
            AccountVotesHistories = new List<AccountVotesHistory>();
            ChatMembers = new List<ChatMember>();
            LikedMovies = new List<LikedMovie>();
            Msgs = new List<Msg>();
            Reviews = new List<Review>();
            Suggestions = new List<Suggestion>();
            UserAuthTokens = new List<UserAuthToken>();
        }

        [Key]
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Required]
        [StringLength(255)]
        public string AccountName { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        public string Pass { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(255)]
        public string ProfilePicture { get; set; }
        public bool IsAdmin { get; set; }
        [Required]
        [StringLength(255)]
        public string Salt { get; set; }
        public int Iterations { get; set; }

        [InverseProperty(nameof(AccountVotesHistory.Account))]
        public virtual List<AccountVotesHistory> AccountVotesHistories { get; set; }
        [InverseProperty(nameof(ChatMember.Account))]
        public virtual List<ChatMember> ChatMembers { get; set; }
        [InverseProperty(nameof(LikedMovie.Account))]
        public virtual List<LikedMovie> LikedMovies { get; set; }
        [InverseProperty(nameof(Msg.Account))]
        public virtual List<Msg> Msgs { get; set; }
        [InverseProperty(nameof(Review.Account))]
        public virtual List<Review> Reviews { get; set; }
        [InverseProperty(nameof(Suggestion.Account))]
        public virtual List<Suggestion> Suggestions { get; set; }
        [InverseProperty(nameof(UserAuthToken.Account))]
        public virtual List<UserAuthToken> UserAuthTokens { get; set; }
    }
}
