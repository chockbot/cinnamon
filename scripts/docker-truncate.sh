sudo docker exec -it cinnamon_db_dev psql -d CINNAMON -U postgres -W postgres -c \
"TRUNCATE TABLE \"WaitLists\", \"SearchTags\", \"Schedules\", \"Descriptions\", \
\"Addresses\", \"ActivityImages\", \"FamilyMembers\", \"Customers\", \"ActivityTypes\", \"Activities\"; \
TRUNCATE TABLE \"AspNetRoleClaims\", \"AspNetUserClaims\", \"AspNetUserLogins\", \
\"AspNetUserRoles\", \"AspNetUserTokens\", \"AspNetRoles\", \"AspNetUsers\";"