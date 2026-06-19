-- Two issues fixed here:
-- 1. "type hotel_role does not exist" — supabase_auth_admin's default
--    search_path doesn't include public, so the bare type name in the
--    function body couldn't resolve. Fixed with SET search_path = public.
-- 2. Claim name collision — Supabase's JWT already has a built-in "role"
--    claim fixed to "authenticated" (used internally for RLS). Our hook
--    was overwriting/competing with that, so logins always read back
--    "authenticated" instead of the real hotel role. Renamed to "user_role".

CREATE OR REPLACE FUNCTION public.custom_access_token_hook(event jsonb)
RETURNS jsonb
LANGUAGE plpgsql
STABLE
SET search_path = public
AS $$
DECLARE
    claims jsonb;
    user_role hotel_role;
    user_branch uuid;
BEGIN
    SELECT role, branch_id INTO user_role, user_branch
    FROM public.user_profiles
    WHERE id = (event->>'user_id')::uuid;

    claims := event->'claims';

    IF user_role IS NOT NULL THEN
        claims := jsonb_set(claims, '{user_role}', to_jsonb(user_role::text));
    ELSE
        claims := jsonb_set(claims, '{user_role}', to_jsonb('Guest'::text));
    END IF;

    IF user_branch IS NOT NULL THEN
        claims := jsonb_set(claims, '{branch_id}', to_jsonb(user_branch::text));
    END IF;

    event := jsonb_set(event, '{claims}', claims);
    RETURN event;
END;
$$;

GRANT EXECUTE ON FUNCTION public.custom_access_token_hook TO supabase_auth_admin;
GRANT SELECT ON public.user_profiles TO supabase_auth_admin;
