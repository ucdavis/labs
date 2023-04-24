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
}