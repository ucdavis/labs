public static class Queries
{
    public static string Terms = @"
        select * from openquery (sis, '
            select stvterm_code as code, stvterm_desc as description, stvterm_start_date as startdate, stvterm_end_date as enddate from stvterm
            where stvterm_end_date > sysdate
            and rownum< 20
            order by stvterm_start_date asc
        ')";

    public static string StudentsForCourse(string crn, string term)
    {
        string query = $@"select * from openquery (sis, '
            select sfrstcr_term_code, sfrstcr_crn, spriden_first_name as firstname, spriden_last_name as lastname, spriden_id as id, lower(wormoth_login_id) loginid
            from sfrstcr
            inner join spriden on sfrstcr_pidm = spriden_pidm
            inner join wormoth on sfrstcr_pidm = wormoth_pidm
            where sfrstcr_term_code = { term }
            and sfrstcr_crn = { crn }
            and sfrstcr_rsts_code in (''RE'', ''RW'')
            and SPRIDEN_CHANGE_IND is null
            and wormoth_acct_type = ''Z''
            and wormoth_acct_status = ''A''
            and wormoth_activity_date = ( select max(wormoth_activity_date ) from wormoth iw where iw.WORMOTH_PIDM = wormoth.wormoth_pidm and iw.wormoth_acct_type = ''Z'' and iw.wormoth_acct_status = ''A'' )
')";

        return query;
    }

    public static string CardholdInfo = @"
    SELECT [CardsID], cardholders.[nCardholderID], [strEmployeeID], [strFirstName], [strLastName], pulldownlists.[strFieldValue] as Department,
    [strEncodedCardNumber], [dtExpirationDate], [nActive], accesslevels1.strAccessLevelName Access1, accesslevels2.strAccessLevelName Access2,
    [nFacilityCode], [strCardFormatName]
    FROM [MILLENNIUM].[iAccess2_1].[dbo].[Cards] cards
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[Cardholders] cardholders on cardholders.[nCardholderID] = cards.[nCardholderID]
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[AccessLevels] accesslevels1 on accesslevels1.[nAccessLevelID] = cards.[nAccessLevel1]
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[AccessLevels] accesslevels2 on accesslevels2.[nAccessLevelID] = cards.[nAccessLevel2]
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[PulldownLists] pulldownlists on pulldownlists.[nFieldID] = cardholders.[nDepartment]
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[CardFormats] cardformats on cardformats.[nCardFormatID] = cards.[CardFormatID]
    where cardholders.[strEmployeeID] in @ids
";
}