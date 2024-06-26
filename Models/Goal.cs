﻿using PennyPlanner.Enums;
using System.Text.Json.Serialization;

namespace PennyPlanner.Models
{
    public class Goal : BaseEntity
    {
        public string Name { get; set; } = default!;
        public GoalType GoalType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float TargetValue { get; set; }
        public float CurrentValue { get; set; }
        public bool IsAchieved { get; set; } = false;
        public Account? Account { get; set; }
        [JsonIgnore]
        public User User { get; set; } = default!;
    }
}
