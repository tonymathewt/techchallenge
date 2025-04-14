MERGE INTO [User] AS target
USING (VALUES
    ('Bob', 'bob@gmail.com', 15),
    ('Jim', 'jim@gmail.com', 45),
    ('Anne', 'anne@gmail.com', 80)
) AS source (Name, Email, CreditRating)
ON target.[UserName] = source.Name
WHEN MATCHED THEN
    UPDATE SET target.CreditRating = source.CreditRating
WHEN NOT MATCHED THEN
    INSERT ([UserName], Email, CreditRating) 
    VALUES (source.Name, source.Email, source.CreditRating);

MERGE INTO Account AS target
USING (VALUES
    (1, 1500),
    (0, 4500),
    (1, 8000)
) AS source (AccountType, Balance)
ON target.AccountType = source.AccountType
	AND target.Balance = source.Balance
WHEN MATCHED THEN
    UPDATE SET target.Balance = source.Balance
WHEN NOT MATCHED THEN
    INSERT (AccountType, Balance) 
    VALUES (source.AccountType, source.Balance);

/* Only meant to be run with 0 intial seed value */
INSERT INTO UserAccount (UserId, AccountId, CreatedDate)
SELECT u.UserId, a.AccountId, GETDATE()
FROM (
    SELECT TOP 3 UserId
    FROM [User]
    ORDER BY UserId 
) u
INNER JOIN (
    SELECT TOP 3 AccountId
    FROM Account
    ORDER BY AccountId 
) a
ON u.UserId = a.AccountId
WHERE NOT EXISTS (
    SELECT 1
    FROM UserAccount l
    WHERE l.UserId = u.UserId AND l.AccountId = a.AccountId
);
