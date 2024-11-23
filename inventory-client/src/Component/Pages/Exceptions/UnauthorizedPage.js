import { t } from "i18next"
import { useTranslation } from "react-i18next";

export default function UnauthorizedPage() {
    const {t} = useTranslation()
    return (
        <div className="unauthorized-page">
            {t('No auth')}
        </div>
    );
}