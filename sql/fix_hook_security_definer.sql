-- The hook runs as supabase_auth_admin which is blocked by user_profiles RLS
-- even with GRANT SELECT. SECURITY DEFINER makes the function execute with
-- the privileges of its owner (postgres) instead, bypassing RLS entirely.

CREATE OR REPLACE FUNCTION public.custom_access_token_hook(event jsonb)
RETURNS jsonb
LANGUAGE plpgsql
SECURITY DEFINER
STABLE
SET search_path = public
AS $$
DECLARE
    v_role   text;
    v_branch uuid;
    claims   jsonb;
BEGIN
    SELECT role::text, branch_id
    INTO v_role, v_branch
    FROM public.user_profiles
    WHERE id = (event->>'user_id')::uuid;

    claims := event->'claims';

    IF v_role IS NOT NULL THEN
        claims := jsonb_set(claims, '{user_role}', to_jsonb(v_role));
    ELSE
        claims := jsonb_set(claims, '{user_role}', to_jsonb('Guest'::text));
    END IF;

    IF v_branch IS NOT NULL THEN
        claims := jsonb_set(claims, '{branch_id}', to_jsonb(v_branch::text));
    END IF;

    event := jsonb_set(event, '{claims}', claims);
    RETURN event;
END;
$$;

GRANT EXECUTE ON FUNCTION public.custom_access_token_hook TO supabase_auth_admin;
GRANT SELECT ON public.user_profiles TO supabase_auth_admin;
