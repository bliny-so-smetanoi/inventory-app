import { t } from "i18next"
import { useTranslation } from "react-i18next";

export default function NoAccessPage() {
    const {t} = useTranslation()
    return (
        <div className="no-access-page">
            {t('No access for this functionality!')}
        </div>
    );
}