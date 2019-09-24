public static class Queries
{
    public static string Terms = @"
        select * from openquery (sis, '
            select stvterm_code as code, stvterm_desc as description, stvterm_start_date as startdate, stvterm_end_date as enddate from stvterm
            where stvterm_end_date > sysdate
            and (stvterm_trmt_code = ''Q'' or stvterm_trmt_code = ''W'')
            and rownum< 20
            order by stvterm_start_date asc
        ')";

        public static string StudentsForCourse(string[] crns, string term)
    {
        string inClause = "(" + string.Join(",", crns) + ")";
        string query = $@"select * from openquery (sis, '
                        select
                            sfrstcr_term_code as code,
                            spriden_id as id,
                            spriden_last_name as lastName,
                            spriden_first_name AS firstName,
                            goremal_email_address AS email,
                            sfrstcr_crn AS CRN,
                            SGVCLAS_LEVL_CODE AS program
                        from spriden
                        inner join sfrstcr on spriden_pidm = sfrstcr_pidm
                        inner join sgvclas on spriden_pidm = SGVCLAS_PIDM
                            and sfrstcr_term_code = SGVCLAS_TERM_CODE
                        left join goremal on goremal_pidm = spriden_pidm
                        and goremal_status_ind = ''A''
                        and goremal_emal_code = ''UCD''
                        where spriden_change_ind IS NULL
                        and sfrstcr_term_code = { term }
                        and sfrstcr_crn in { inClause }
                        and sfrstcr_rsts_code = ''RE''
                        ')";

        return query;
    }

    public static string CardholdInfo = @"
    SELECT [CardsID], cardholders.[nCardholderID], [strEmployeeID], [strFirstName], [strLastName], pulldownlists.[strFieldValue] as Department,
    [strEncodedCardNumber], [dtExpirationDate], [nActive], accesslevels1.strAccessLevelName Access1, accesslevels2.strAccessLevelName Access2,
    [nFacilityCode], [strCardFormatName]
    FROM [MILLENNIUM].[iAccess2_1].[dbo].[Cards] cards
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[Cardholders] cardholders on cardholders.[nObjectID] = cards.[nCardholderID]
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[AccessLevels] accesslevels1 on accesslevels1.[nAccessLevelID] = cards.[nAccessLevel1]
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[AccessLevels] accesslevels2 on accesslevels2.[nAccessLevelID] = cards.[nAccessLevel2]
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[PulldownLists] pulldownlists on pulldownlists.[nFieldID] = cardholders.[nDepartment]
    inner join [MILLENNIUM].[iAccess2_1].[dbo].[CardFormats] cardformats on cardformats.[nCardFormatID] = cards.[CardFormatID]
    where cardholders.[strEmployeeID] in @ids
";
}